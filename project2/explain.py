import psycopg2

# CHANGE HERE
conn = psycopg2.connect(
    host="localhost",
    database="TPC-H",
    user="postgres",
    password="postgres"
)

# CHANGE HERE
query1 = "SELECT * FROM customer WHERE c_acctbal < 500 AND c_nationkey = 1;"
query2 = "SELECT * FROM customer WHERE c_nationkey = 1;"

cur = conn.cursor()

cur.execute("EXPLAIN (FORMAT JSON) " + query1)
plan1 = cur.fetchall()
print("Plan 1:")
for row in plan1:
    print(row)

cur.execute("EXPLAIN (FORMAT JSON) " + query2)
plan2 = cur.fetchall()
print("Plan 2:")
for row in plan2:
    print(row)

# diff = set(plan1).difference(set(plan2))
# print("Difference:")
# for row in diff:
#     print(row)

cur.close()
conn.close()
