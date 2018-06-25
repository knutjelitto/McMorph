FROM microsoft/dotnet:2.1.300-sdk-alpine

RUN apk update && apk upgrade && apk add --no-cache \
    man man-pages \
    bash bash-doc \
    less less-doc \
    lzip lzip-doc \
    tar tar-doc

ENV TERM=xterm
ENV PATH=/sbin:/bin:/usr/sbin:/usr/bin:/root/LiFo/bin

RUN ln -sf bash /bin/sh

WORKDIR /root

CMD [ "/bin/bash",  "-l" ]
