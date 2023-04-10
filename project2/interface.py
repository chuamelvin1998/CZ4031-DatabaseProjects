import PySimpleGUI as sg
from explain import *
import psycopg2
import json 

def run_sql_query_compare():
    conn = psycopg2.connect(
    host="localhost",
    database="TPC-H",
    user="postgres",
    password="postgres"
)
    
    left_col = [
        [sg.Text("Old Query")],[sg.Multiline(key='query-input1', size=(50, 5))]
    ]

    right_col = [
        [sg.Text("New Query")],[sg.Multiline(key='query-input2', size=(50, 5))]
    ]

    canvasSize = 355

    left_col2 = [
        [sg.Text("Old Query Visual Description")],
        [sg.Graph(canvas_size=(canvasSize, canvasSize), graph_bottom_left=(0, 0), graph_top_right=(canvasSize, canvasSize), background_color='white', key='canvas1')],
        [sg.Text("Old Query Text Description")],
        [sg.Multiline(key='text-desc1', size=(50, 5),disabled=True)]
    ]

    right_col2 = [
        [sg.Text("New Query Visual Description")],
        [sg.Graph(canvas_size=(canvasSize, canvasSize), graph_bottom_left=(0, 0), graph_top_right=(canvasSize, canvasSize), background_color='white', key='canvas2')],
        [sg.Text("New Query Text Description")],
        [sg.Multiline(key='text-desc2', size=(50, 5),disabled=True)]
    ]

    layout = [[sg.Column(left_col), sg.Column(right_col)],
            [sg.Button("Compare Queries", pad=((330, 330), 10) , key='compare-button')],
            [sg.Text("Description of Change")],
            [sg.Multiline(key='text-desc-change', size=(107, 5), disabled=True)],
            [sg.Column(left_col2), sg.Column(right_col2)]
    ]

    # Create the window and set its title
    window = sg.Window("SQL Query Compare", layout)

    # Loop through events until the window is closed
    while True:
        event, values = window.read()
        
        # If the user closes the window, exit the loop
        if event == sg.WINDOW_CLOSED:
            break
        
        # If the user clicks the Submit button, compare the queries
        if event == "compare-button":
            query1 = values['query-input1']
            query2 = values['query-input2']
            
            # Compare the queries and display the differences in the -DIFF- element
            diff = get_query_plan_diff(conn,query1,query2)
            print(get_query_plan_diff(conn,query1,query2))
            friendlyText = "All these are due to the changes made from the old query to the new query:"
            if (diff.get('Node Type') and diff['Node Type'][0] != diff['Node Type'][1]):
                nodeType = f"It uses {diff['Node Type'][0]} in the old query and has evolved to using {diff['Node Type'][1]} in the new query. "
                friendlyText.append(nodeType)
            if (diff.get('Strategy') and diff['Strategy'][0] != diff['Strategy'][1]):
                if (diff['Strategy'][0] == None):
                    strategyType = f"It does not use any strategy in the old query and has evolved to using {diff['Strategy'][1]} strategy in the new query. "
                elif (diff['Strategy'][1] == None):
                    strategyType = f"It uses {diff['Strategy'][0]} strategy in the old query and has evolved to using no strategy in the new query. "
                else:
                    strategyType = f"It uses {diff['Strategy'][0]} strategy in the old query and has evolved to using {diff['Strategy'][1]} strategy in the new query. "
                friendlyText.append(strategyType)
            if (diff.get('Startup Cost') and diff['Startup Cost'][0] != diff['Startup Cost'][1]):
                startupCost = f"It cost {diff['Startup Cost'][0]} to start up in the old query and has evolved to cost {diff['Startup Cost'][1]} in the new query. "
                friendlyText.append(startupCost)
            if (diff.get('Total Cost') and diff['Total Cost'][0] != diff['Total Cost'][1]):
                totalCost = f"It cost a total of {diff['Total Cost'][0]} in the old query and has evolved to cost {diff['Total Cost'][1]} in the new query. "
                friendlyText.append(totalCost)
            diff = pretty_print_json(json.dumps(diff, indent=4))
            window["text-desc-change"].update(friendlyText)
            # diff = ""


            q1 = explain_query(conn,query1)
            q1_pretty = pretty_print_json(json.dumps(q1, indent=4))
            q2 = explain_query(conn,query2)
            q2_pretty = pretty_print_json(json.dumps(q2, indent=4))
            print(q1)
            print(q2)
            window["text-desc1"].update(q1_pretty)
            window["text-desc2"].update(q2_pretty)
            if query1 != query2:
                # Split the queries into lines and compare them line-by-line
                query1_lines = query1.split("\n")
                query2_lines = query2.split("\n")
                for i in range(max(len(query1_lines), len(query2_lines))):
                    if i >= len(query1_lines) or i >= len(query2_lines):
                        diff += f"{'-'*50}\n"
                        break
                    if query1_lines[i] != query2_lines[i]:
                        diff += f"{query1_lines[i]}\n{'-'*50}\n{query2_lines[i]}\n{'-'*50}\n"
                # window["text-desc-change"].update(diff)
            else:
                diff = "Queries are identical."
                window["text-desc-change"].update(diff)

    # Close the window when the loop ends
    window.close()
    conn.close()
    
def pretty_print_json(json_str):
    obj = json.loads(json_str)
    pretty_str = ""
    for key, value in obj.items():
        pretty_str += f"{key}: {value}\n"
    return pretty_str

if __name__ == '__main__':
    run_sql_query_compare()