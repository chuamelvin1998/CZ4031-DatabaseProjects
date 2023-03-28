import psycopg2

# CHANGE HERE
conn = psycopg2.connect(
    host="localhost",
    database="TPC-H",
    user="postgres",
    password="postgres"
)

# CHANGE HERE
query = "SELECT * FROM customer WHERE c_acctbal < 500 AND c_nationkey = 1;"

cur = conn.cursor()

cur.execute(query)
rows = cur.fetchall()

for row in rows:
    print(row)

cur.execute("EXPLAIN " + query)
plan = cur.fetchall()
for p in plan:
    print(p)

cur.close()
conn.close()
