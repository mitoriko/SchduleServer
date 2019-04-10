FROM mcr.microsoft.com/dotnet/core/runtime:2.1
WORKDIR /app
ADD SchduleServer/obj/Docker/publish /app
RUN cp /usr/share/zoneinfo/Asia/Shanghai /etc/localtime
ENTRYPOINT ["dotnet", "SchduleServer.dll"]
