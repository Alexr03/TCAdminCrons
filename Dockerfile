FROM mono:latest

RUN nuget source Add -Name "TCAdmin" -Source "https://nexus-repository.openshift.alexr03.dev/repository/tcadmin/" && \
    nuget restore -NonInteractive && \
    msbuild /p:Configuration="Release" /p:Platform="Any CPU" /p:OutputPath="./release/" "TCAdminCrons/TCAdminCrons.csproj"
    
COPY release /app

WORKDIR /app

CMD [ "mono", "/app/TCAdminCrons.exe" ]