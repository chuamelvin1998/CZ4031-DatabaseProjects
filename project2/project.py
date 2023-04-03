from interface import run_sql_query_compare
from explain import get_query_plan_diff
import psycopg2

conn = psycopg2.connect(
    host="localhost",
    database="TPC-H",
    user="postgres",
    password="postgres"
)

# TODO OR REMOVE: Example usage of explain.py, move where needed
query1 = "SELECT * FROM customer WHERE c_acctbal < 500 AND c_nationkey = 1;"
query2 = "SELECT * FROM customer WHERE c_nationkey = 1;"
diff = get_query_plan_diff(conn, query1, query2)

run_sql_query_compare()