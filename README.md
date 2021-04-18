# Pokedex

Pokedex is an API layer on top of PokeAPI and FunTranslateAPI which provides endpoints to retrieve Pokemon info in english and also get them in some fun translation such as Yoda or Shakespeare.

The project has been developed on .Net 5 so in order to build/run the project .net 5.0 SDK/runtime need to be installed. You can download them from https://dotnet.microsoft.com/download/dotnet/5.0 depend on your operating system and also your hardware configuration. 

## How to build/run

In order to run the project:

```
git clone https://github.com/moattarwork/pokedex

cd pokedex\src

dotnet build
dotnet .\Pokedex.WebApi\bin\Debug\net5.0\Pokedex.WebApi.dll 
```

Now the application available to run:

```
# pokemon info
curl https://localhost:5000/pokemon/<name>

For example:
    curl https://localhost:5000/pokemon/mewtwo

# pokemon translated info
curl https://localhost:5000/pokemon/translated/<name>

For example:
    curl https://localhost:5000/pokemon/translated/mewtwo
```

### Open API EndPoint

The API contains the Open API endpoint which can be used to test the endpoints in the browser. The Open API is available on http://localhost:5000/swagger 

### Running in the Docker

An alternative way of running the application is to run docker-compose file in the command line:

```
cd pokedex\src

docker-compose up -d

```
**NOTE: to be able to run the application in the docker, the docker desktop must be available on your machine. You can download the docker desktop from https://www.docker.com/products/docker-desktop** 

## Production considerations
- Enabling resiliency on external API calls (Retry, CircuitBreaker, etc) using libraries such as Polly
- Generic poke info is pretty static and it worth to enable a proper caching solution on the API
- Extended logging & enabling telemetry/monitoring
-  
- 
