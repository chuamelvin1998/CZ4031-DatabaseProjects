import tkinter as tk
from explain import *

FIXED_VERTICAL = 75
RECT_WIDTH = 100
RECT_HEIGHT = 50
CANVAS_WIDTH = 700
CANVAS_HEIGHT = 700


def process_query_plan(query):
    plan =[]
    for key in query:
        if key == "Node Type":
            if "Relation Name" in query:
                plan.append(query[key] + "|" + query["Relation Name"])
            else:
                plan.append(query[key])

        elif key == "Plans":
            for subquery in query[key]:
                plan.append(process_query_plan(subquery))

    return plan

def draw_queries(query1, query2):
    root = tk.Tk()
    
    # Create the first canvas and place it on the left side
    canvas1 = tk.Canvas(root, width=CANVAS_WIDTH, height=CANVAS_HEIGHT, bg='white')
    canvas1.grid(row=0, column=0, padx=20, pady=20)

    # Create a frame to hold the widgets inside the first canvas
    frame1 = tk.Frame(canvas1)
    canvas1.create_window(0, 0, window=frame1, anchor='nw')

    # Create vertical scrollbar for the first canvas
    scrollbar1_y = tk.Scrollbar(root, orient='vertical', command=canvas1.yview)
    scrollbar1_y.grid(row=0, column=1, sticky='ns')
    canvas1.config(yscrollcommand=scrollbar1_y.set)

    # Create horizontal scrollbar for the first canvas
    scrollbar1_x = tk.Scrollbar(root, orient='horizontal', command=canvas1.xview)
    scrollbar1_x.grid(row=1, column=0, sticky='we')
    canvas1.config(xscrollcommand=scrollbar1_x.set)

    # Add some widgets to the frame inside the first canvas
    for i in range(50):
        label = tk.Label(frame1, text=f"{i}")
        label.grid(row=i, column=0)

    # Update the scroll region of the first canvas
    frame1.update_idletasks()
    canvas1.config(scrollregion=canvas1.bbox('all'))

    # Create the second canvas and place it on the right side
    canvas2 = tk.Canvas(root, width=CANVAS_WIDTH, height=CANVAS_HEIGHT, bg='white')
    canvas2.grid(row=0, column=2, padx=20, pady=20)

    # Create a frame to hold the widgets inside the second canvas
    frame2 = tk.Frame(canvas2)
    canvas2.create_window(0, 0, window=frame2, anchor='nw')

    # Create vertical scrollbar for the second canvas
    scrollbar2_y = tk.Scrollbar(root, orient='vertical', command=canvas2.yview)
    scrollbar2_y.grid(row=0, column=3, sticky='ns')
    canvas2.config(yscrollcommand=scrollbar2_y.set)

    # Create horizontal scrollbar for the second canvas
    scrollbar2_x = tk.Scrollbar(root, orient='horizontal', command=canvas2.xview)
    scrollbar2_x.grid(row=1, column=2, sticky='we')
    canvas2.config(xscrollcommand=scrollbar2_x.set)

    # Add some widgets to the frame inside the second canvas
    for i in range(50):
        label = tk.Label(frame2, text=f"{i}")
        label.grid(row=i, column=0)

    # Update the scroll region of the second canvas
    frame2.update_idletasks()
    canvas2.config(scrollregion=canvas2.bbox('all'))

    plan1 = process_query_plan(query1)
    plan2 = process_query_plan(query2)

    print(plan1)
    print(plan2)

    draw_plan(canvas1, plan1)
    draw_plan(canvas2, plan2)
    
    # root.mainloop()
    
def draw_plan(canvas,plan):
    county = 0
    countx = 1
    while plan:
        print("plan: ",plan)
        node = plan.pop(0)
        if type(node) is list:
            for subnode in node:
                if("|" in subnode):
                    temp = subnode.split("|")
                    plan.append(temp[0])
                    plan.append(temp[1])
                elif "Join" in subnode:
                    draw_rectangle(canvas, subnode, county)
                    county+=1
                    # left = subnode[1]
                    # draw_plan(canvas,left)
                    # right = subnode[2]
                    # draw_plan(canvas,right)
                else:
                    plan.append(subnode)
        else:
            draw_rectangle(canvas, node, county)
            county+=1


def draw_rectangle(canvas, text, county):
    color = ""

    if "Scan" in text:
        color = "light green"
    elif "Join" in text:
        color = "light blue"
    else:
        color = "light grey"

    canvas_x = (CANVAS_WIDTH - RECT_WIDTH) / 2
    rect = canvas.create_rectangle(canvas_x, 10+county*FIXED_VERTICAL, canvas_x+RECT_WIDTH, RECT_HEIGHT+county*FIXED_VERTICAL, fill=color)
    rect_coords = canvas.bbox(rect)
    rect_center_x = (rect_coords[0] + rect_coords[2]) / 2
    rect_center_y = (rect_coords[1] + rect_coords[3]) / 2
    canvas.create_text(rect_center_x, rect_center_y, text=text, fill='black')


