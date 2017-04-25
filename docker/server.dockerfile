FROM microsoft/dotnet:1.1.1-runtime

# Install the required applications
RUN apt-get update --fix-missing && \
    apt-get upgrade -y && \
    apt-get install -y \
        supervisor \
        nginx

# Set the Environment
ENV ASPNETCORE_ENVIRONMENT=Production

# Copy the configuration
COPY ./conf/supervisor.conf /home/docker/conf/supervisor.conf
COPY ./conf/nginx.conf /etc/nginx/nginx.conf
COPY ./conf/proxy.conf /etc/nginx/proxy.conf

# Create the log directory
RUN mkdir -p /home/docker/logs

# Copy the application
COPY ./artifacts/server/ /home/docker/app/

# Set the working directory
WORKDIR /home/docker/app

RUN mkdir -p /home/docker/app/logs

# Expose the webserver port
EXPOSE 80

# Run Supervisor
CMD ["supervisord", "-n", "-c", "/home/docker/conf/supervisor.conf"]