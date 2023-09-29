FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src

# copy csproj and restore as distinct layers
COPY *.props .
COPY *.sln .

# copy all the projects from host to image
# Base
COPY Base.Contracts.BLL/*.csproj ./Base.Contracts.BLL/
COPY Base.Contracts/*.csproj ./Base.Contracts/
COPY Base.Contracts.DAL/*.csproj ./Base.Contracts.DAL/
COPY Base.Contracts.Domain/*.csproj ./Base.Contracts.Domain/
COPY Base.BLL/*.csproj ./Base.BLL/
COPY Base.DAL/*.csproj ./Base.DAL/
COPY Base.DAL.EF/*.csproj ./Base.DAL.EF/
COPY Base.Domain/*.csproj ./Base.Domain/
COPY Base.Helpers/*.csproj ./Base.Helpers/

# App
COPY App.Contracts.BLL/*.csproj ./App.Contracts.BLL/
COPY App.Contracts.DAL/*.csproj ./App.Contracts.DAL/
COPY App.BLL/*.csproj ./App.BLL/
COPY App.BLL.DTO/*.csproj ./App.BLL.DTO/
COPY App.DAL.EF/*.csproj ./App.DAL.EF/
COPY App.Domain/*.csproj ./App.Domain/
COPY App.Public.DTO/*.csproj ./App.Public.DTO/
COPY WebApp/*.csproj ./WebApp/

COPY Tests.WebApp/*.csproj ./Tests.WebApp/

RUN dotnet restore

# Copy everything else and build app
# Base
COPY Base.Contracts.BLL/. ./Base.Contracts.BLL/
COPY Base.Contracts/. ./Base.Contracts/
COPY Base.Contracts.DAL/. ./Base.Contracts.DAL/
COPY Base.Contracts.Domain/. ./Base.Contracts.Domain/
COPY Base.BLL/. ./Base.BLL/
COPY Base.DAL/. ./Base.DAL/
COPY Base.DAL.EF/. ./Base.DAL.EF/
COPY Base.Domain/. ./Base.Domain/
COPY Base.Helpers/. ./Base.Helpers/

# App
COPY App.Contracts.BLL/. ./App.Contracts.BLL/
COPY App.Contracts.DAL/. ./App.Contracts.DAL/
COPY App.BLL/. ./App.BLL/
COPY App.BLL.DTO/. ./App.BLL.DTO/
COPY App.DAL.EF/. ./App.DAL.EF/
COPY App.Domain/. ./App.Domain/
COPY App.Public.DTO/. ./App.Public.DTO/
COPY WebApp/. ./WebApp/

COPY Tests.WebApp/. ./Tests.WebApp/

WORKDIR /src/WebApp
RUN dotnet publish -c Release -o out


FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
EXPOSE 80
COPY --from=build /src/WebApp/out ./
ENTRYPOINT ["dotnet", "WebApp.dll"]