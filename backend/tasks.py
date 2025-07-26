import os
from invoke import task


@task
def dev(ctx):
    ctx.run(
        "uvicorn main:app --host 0.0.0.0 --port 8001 --reload",
        pty=os.name != "nt",
        env={"APP_ENV": "development"},
    )
