ARG BASE=mcr.microsoft.com/dotnet/aspnet:6.0.3
FROM $BASE AS base

WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY /src/my.services.contactinfo.Web/bin/Release/net6.0/publish/ .

# In future, we will move the following COPY commands into platform container base docker file
COPY /us-east-2-bundle.pem .
COPY /us-west-2-bundle.pem .
COPY Datadog.Linux.ApiWrapper.x64.so /opt/datadog/./linux-x64/
COPY ld.so.preload /etc/ld.so.preload

ENTRYPOINT ["dotnet", "my.services.contactinfo.Web.dll"]