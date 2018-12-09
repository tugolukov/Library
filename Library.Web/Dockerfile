FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
ARG source

#build
FROM microsoft/dotnet:2.1-sdk as build
WORKDIR /app
COPY /bin/Release/publish .
COPY wwwroot/ /wwwroot

# run
WORKDIR /app
ARG Environment=Work
ENV ASPNETCORE_ENVIRONMENT $Environment
ENV ASPNETCORE_URLS http://*:80
EXPOSE 80
ENTRYPOINT ["dotnet", "Library.Web.dll"] 