FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /source

COPY *.sln .
COPY */*.csproj .

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
COPY . .
RUN dotnet publish ./LibraryApi.Identity.Web/LibraryApi.Identity.Web.csproj -c release --no-restore;

# move results of app publishing into single folder for convenience
RUN mv ./LibraryApi.Identity.Web/bin/release/net8.0 ./app;


#for next stage of image building use another baseimage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build source/app ./
COPY ./Certs ./Certs

ENTRYPOINT ["dotnet", "LibraryApi.Identity.Web.dll"]