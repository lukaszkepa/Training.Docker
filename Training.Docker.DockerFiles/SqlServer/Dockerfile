FROM microsoft/mssql-server-linux AS base
EXPOSE 1433

ENV MSSQL_SA_PASSWORD=Passw@rd
ENV ACCEPT_EULA=Y

COPY Training.Docker.DockerFiles/SqlServer/Scripts/*.sql /scripts/

RUN /opt/mssql/bin/sqlservr & sleep 20 \
    && /opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U sa -P Passw@rd -i /scripts/CreateDockrCourseDb.sql \
    && pkill sqlservr
