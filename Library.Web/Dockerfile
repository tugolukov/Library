FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
ARG source

#build
FROM microsoft/dotnet:2.2-sdk as build
WORKDIR /app
COPY /bin/Debug/netcoreapp2.2/publish .
COPY wwwroot/ /wwwroot

# run
WORKDIR /app
ARG Environment=Work
ENV ASPNETCORE_ENVIRONMENT $Environment
ENV ASPNETCORE_URLS http://*:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "Library.Web.dll"] 