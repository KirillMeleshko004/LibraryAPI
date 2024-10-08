FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /source

COPY ./LibraryService/*.sln .
COPY ./LibraryService/*/*.csproj .

# create directory for each .csproj file and move each .csproj to corresponding directory
RUN for file in $(ls *.csproj); do \
   # each file is like {filename}.csproj
   # ${var%string} Delete the shortest match of string in $var from the end
   # ${file%.*} is simple {filename}
   mkdir -p ./${file%.*}/ && mv $file ./${file%.*}/; \
   done;

# restore all depen  dencies in .csproj files according to .sln file
RUN dotnet restore;


# copy all files and publish application
# default publishing folder - ./{ProjectName}/bin/{Enviroment}/{.net version}
COPY ./LibraryService/. .

RUN dotnet test ./Library.Test/Library.Test.csproj --no-restore
RUN dotnet publish ./Library.Api/Library.Api.csproj -c release --no-restore;

# move results of app publishing into single folder for convenience
RUN mv ./Library.Api/bin/release/net8.0 ./app;

#cert creation
WORKDIR /certs

COPY ./CertCreator/*.sln .
COPY ./CertCreator/*.csproj .
COPY ./CertCreator/. .

RUN dotnet restore;
RUN dotnet publish ./CertCreator.csproj -c release --no-restore;
RUN dotnet ./bin/release/net8.0/CertCreator.dll

#for next stage of image building use another baseimage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build source/app ./
COPY --from=build certs/Certs ./Certs

RUN mkdir /app/Data

ENTRYPOINT ["dotnet", "Library.Api.dll"]