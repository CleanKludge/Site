FROM microsoft/dotnet:1.1.1-runtime

# Install the required applications
RUN apt-get update --fix-missing && \
    apt-get upgrade -y && \
    apt-get install -y \
        supervisor

# Set the Environment
ENV ASPNETCORE_ENVIRONMENT=Production

# Copy the configuration
COPY ./conf/supervisor.conf /home/docker/conf/supervisor.conf

# Create the log directory for supervisor
RUN mkdir -p /home/docker/logs

# Copy the application
COPY ./artifacts/server/ /home/docker/app/

# Set the working directory
WORKDIR /home/docker/app

# Create the log directory for the application
RUN mkdir -p /home/docker/app/logs

# Expose the webserver port
EXPOSE 80

# Run Supervisor
CMD ["supervisord", "-n", "-c", "/home/docker/conf/supervisor.conf"]