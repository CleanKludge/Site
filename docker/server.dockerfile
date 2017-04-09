FROM microsoft/dotnet:1.1.0-runtime

# Install the required applications
RUN apt-get update --fix-missing && \
    apt-get upgrade -y && \
    apt-get install -y \
        supervisor \
        nginx

# Copy the application
COPY ./artifacts/server/ /home/docker/app/

WORKDIR /home/docker/app

ENV ASPNETCORE_ENVIRONMENT=Production

# Copy the configuration
COPY ./conf/supervisor.conf /home/docker/conf/supervisor.conf
COPY ./conf/nginx.conf /etc/nginx/nginx.conf
COPY ./conf/proxy.conf /etc/nginx/proxy.conf

# Create the log directory
RUN mkdir -p /home/docker/logs

EXPOSE 80

CMD ["supervisord", "-n", "-c", "/home/docker/conf/supervisor.conf"]