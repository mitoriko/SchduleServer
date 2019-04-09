FROM microsoft/dotnet:2.1-runtime
WORKDIR /app
ADD SchduleServer/obj/Docker/publish /app
RUN cp /usr/share/zoneinfo/Asia/Shanghai /etc/localtime
ENTRYPOINT ["dotnet", "SchduleServer.dll"]
