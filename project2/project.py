from interface import run_sql_query_compare
from explain import get_query_plan_diff, explain_query
import psycopg2

conn = psycopg2.connect(
    host="localhost",
    database="TPC-H",
    user="postgres",
    password="postgres"
)

run_sql_query_compare()

conn.close()

