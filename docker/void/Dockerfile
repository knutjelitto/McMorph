FROM voidlinux/voidlinux-musl as builder

RUN mkdir /tmp/rootfs && \
    yes | XBPS_ARCH=x86_64-musl xbps-install \
        --yes \
        --rootdir /tmp/rootfs \
        --repository=http://muslrepo.voidlinux.eu/current \
        --sync \
        base-files \
        bash \
        ca-certificates \
        xbps \
        coreutils \
        findutils \
        diffutils \
        ncurses \
        grep \
        gzip \
        file \
        sed \
        gawk \
        less \
        util-linux \
        which \
        tar \
        man-pages \
        mdocml \
        psmisc \
        procps-ng \
        tzdata \
        iana-etc

FROM scratch

COPY --from=builder /tmp/rootfs /

ENV TERM=xterm

RUN xbps-alternatives --set bash && \
    rm -rv /var/cache/xbps/* && \
    mv /etc/profile.d/colorls.sh /etc/profile.d/colorls.sh-disabled

CMD [ "/bin/bash",  "-l" ]