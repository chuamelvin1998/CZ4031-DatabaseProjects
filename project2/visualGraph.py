import tkinter as tk
from explain import *

FIXED_VERTICAL = 75
FIXED_HORIZONTAL = 100
RECT_WIDTH = 100
RECT_HEIGHT = 50
CANVAS_WIDTH = 700
CANVAS_HEIGHT = 700
ALL_RECTANGLES_CANVAS1=[]
ALL_RECTANGLES_CANVAS2=[]

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
    canvas1 = tk.Canvas(root, width=CANVAS_WIDTH, height=CANVAS_HEIGHT, bg='white', name="canvas1")
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
    canvas2 = tk.Canvas(root, width=CANVAS_WIDTH, height=CANVAS_HEIGHT, bg='white',name="canvas2")
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

    # print(plan1)
    # print(plan2)

    array1 =  draw_plan(canvas1, plan1)
    array2  = draw_plan(canvas2, plan2)

    ALL_RECTANGLES_CANVAS1 = (array1)
    ALL_RECTANGLES_CANVAS2 = (array2)

    print(ALL_RECTANGLES_CANVAS1)

    draw_arrows(canvas1,ALL_RECTANGLES_CANVAS1)
    draw_arrows(canvas2,ALL_RECTANGLES_CANVAS2)

    # root.mainloop()

def draw_arrows(canvas, array):
    firstRectIndex = 0
    secondRectIndex = 1

    while secondRectIndex < len(array):
        rect1 = array[firstRectIndex]
        # print("stpt",rect1)
        startpt = rect_bottom_center(rect1[0],rect1[1],rect1[2],rect1[3])

        rect2 = array[secondRectIndex]
        # print("endpt",rect2)
        if type(rect2) is list:
            rect2 = array[secondRectIndex][0]
            endpt = rect_top_center(rect2[0],rect2[1],rect2[2],rect2[3])
            canvas.create_line(startpt[0],startpt[1],endpt[0],endpt[1])

            rect1 = array[secondRectIndex][0]
            startpt = rect_bottom_center(rect1[0],rect1[1],rect1[2],rect1[3])

            rect2 = array[secondRectIndex][1][0]
            endpt = rect_top_center(rect2[0],rect2[1],rect2[2],rect2[3])
            canvas.create_line(startpt[0],startpt[1],endpt[0],endpt[1])

            rect2 = array[secondRectIndex][2][0]
            endpt = rect_top_center(rect2[0],rect2[1],rect2[2],rect2[3])
            canvas.create_line(startpt[0],startpt[1],endpt[0],endpt[1])
        
            draw_arrows(canvas, array[secondRectIndex][1])
            draw_arrows(canvas, array[secondRectIndex][2])
        elif type(rect2) is tuple:
            endpt = rect_top_center(rect2[0],rect2[1],rect2[2],rect2[3])
            canvas.create_line(startpt[0],startpt[1],endpt[0],endpt[1])
        firstRectIndex+=1
        secondRectIndex+=1

def rect_top_center(x1, y1, x2, y2):
    return (x1 + x2) / 2, y1
    
def rect_bottom_center(x1, y1, x2, y2):
    return (x1 + x2) / 2, y2

def draw_plan(canvas,plan,countx=4,county=0):
    countx =countx
    county = county
    temparray = []
    while plan:
        node = plan.pop(0)
        print(node)
        if type(node) is list:
            if "Join" in node[0]:
                coords = draw_rectangle(canvas, node[0],countx, county)
                county+=1
                left = node[1]
                right = node[2]
                leftArray = draw_plan(canvas,left,countx-1,county)
                rightArray = draw_plan(canvas,right,countx+1,county)

                temparray.append([coords,leftArray,rightArray])

            else:
                for subnode in node:
                    plan.append(subnode)
        elif ("|" in node):
            temp = node.split("|")
            coords1 = draw_rectangle(canvas, temp[0],countx, county)
            county+=1
            coords2 = draw_rectangle(canvas, temp[1] ,countx, county)
            county+=1

            temparray.append(coords1)
            temparray.append(coords2)
        else:
            coords = draw_rectangle(canvas, node,countx, county)
            county+=1

            temparray.append(coords)
    
    return temparray


def draw_rectangle(canvas, text,countx, county):
    color = ""

    if "Scan" in text:
        color = "light green"
    elif "Join" in text:
        color = "light blue"
    else:
        color = "light grey"

    # canvas_x = (CANVAS_WIDTH - RECT_WIDTH) / 2
    rect = canvas.create_rectangle(countx*FIXED_HORIZONTAL, 10+county*FIXED_VERTICAL,RECT_WIDTH+countx*FIXED_HORIZONTAL, RECT_HEIGHT+county*FIXED_VERTICAL, fill=color)
    rect_coords = canvas.bbox(rect)
    rect_center_x = (rect_coords[0] + rect_coords[2]) / 2
    rect_center_y = (rect_coords[1] + rect_coords[3]) / 2
    canvas.create_text(rect_center_x, rect_center_y, text=text, fill='black')

    return rect_coords




