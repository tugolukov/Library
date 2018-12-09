FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
ARG source

#build
FROM microsoft/dotnet:2.2-sdk as build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY */*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out


COPY /bin/Release/publish .
COPY wwwroot/ /wwwroot

# run
WORKDIR /app
ARG Environment=Work
ENV ASPNETCORE_ENVIRONMENT $Environment
ENV ASPNETCORE_URLS http://*:80
EXPOSE 80
ENTRYPOINT ["dotnet", "Library.Web.dll"] 