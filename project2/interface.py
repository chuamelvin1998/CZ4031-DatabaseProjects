import PySimpleGUI as sg

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
    [sg.Text("Old Query Text Description")],
    [sg.Multiline(key='text-desc2', size=(50, 5),disabled=True)]
]

layout = [[sg.Column(left_col), sg.Column(right_col)],
          [sg.Button("Compare Queries", pad=((330, 330), 10) , key='compare-button')],
          [sg.Text("Description of Change")],
          [sg.Multiline(key='text-desc-change', size=(107, 5), disabled=True)],
          [sg.Column(left_col2), sg.Column(right_col2)],
          
          
]

# Create the window and set its title
window = sg.Window("SQL Query Comparator", layout)

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
        diff = ""
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
        else:
            diff = "Queries are identical."
        window["text-desc-change"].update(diff)

# Close the window when the loop ends
window.close()
