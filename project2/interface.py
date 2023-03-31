import PySimpleGUI as sg

# Define the layout of the interface
layout = [[sg.Text("Query 1"), sg.InputText()],
          [sg.Text("Query 2"), sg.InputText()],
          [sg.Button("Submit")],
          [sg.Multiline(size=(60, 10), key="-DIFF-")]]

# Create the window and set its title
window = sg.Window("SQL Query Comparator", layout)

# Loop through events until the window is closed
while True:
    event, values = window.read()
    
    # If the user closes the window, exit the loop
    if event == sg.WINDOW_CLOSED:
        break
    
    # If the user clicks the Submit button, compare the queries
    if event == "Submit":
        query1 = values[0]
        query2 = values[1]
        
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
        window["-DIFF-"].update(diff)

# Close the window when the loop ends
window.close()
