FROM mono:latest

COPY . /src
WORKDIR /src

RUN nuget source Add -Name "TCAdmin" -Source "https://nexus-repository.openshift.alexr03.dev/repository/tcadmin/index.json" && \
    nuget restore -NonInteractive && \
    xbuild /p:Configuration=Release /p:OutputPath="/app/"
    
WORKDIR /app
    
CMD [ "mono", "/app/TCAdminCrons.exe" ]