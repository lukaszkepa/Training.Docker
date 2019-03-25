#!/bin/sh

: ${MONGO_HOST:=172.17.0.2}
: ${MONGO_PORT:=1433}

until nc -z $MONGO_HOST $MONGO_PORT
do
    echo "Waiting for SqlServer ($MONGO_HOST:$MONGO_PORT) to start..."
    sleep 0.5
done

eval $*