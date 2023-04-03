"""
Returns a dictionary of the differences between two query plans in the format: 
{key_containing_change: [query1_value, query2_value]}
"""

def get_query_plan_diff(conn, query1, query2):
    cur = conn.cursor()
    cur.execute("EXPLAIN (FORMAT JSON) " + query1)
    plan1 = cur.fetchall()[0][0][0]["Plan"]
    print("Plan 1: ", plan1)
    print()

    cur.execute("EXPLAIN (FORMAT JSON) " + query2)
    plan2 = cur.fetchall()[0][0][0]["Plan"]
    print("Plan 2: ", plan2)
    print()

    cur.close()
    conn.close()

    diff = {}

    for key in plan1:
        if key not in plan2:
            diff[key] = [plan1[key], None]
        elif plan1[key] != plan2[key]:
            diff[key] = [plan1[key], plan2[key]]
    for key2 in plan2:
        if key2 not in plan1:
            diff[key2] = [None, plan2[key2]]
    
    print("Differences: ", diff)

    return diff


