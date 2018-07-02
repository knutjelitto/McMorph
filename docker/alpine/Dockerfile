FROM microsoft/dotnet:2.1-sdk-alpine

RUN apk update && apk upgrade && apk add --no-cache \
    man man-pages bash less lzip tree file build-base \
    docs

ENV TERM=xterm
ENV PATH=/sbin:/bin:/usr/sbin:/usr/bin:/root/LiFo/bin

RUN ln -sf bash /bin/sh

WORKDIR /root

CMD [ "/bin/bash",  "-l" ]
