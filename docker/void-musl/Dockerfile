FROM voidlinux/voidlinux-musl

ENV TERM=xterm

RUN xbps-install --sync --update --yes && \
    xbps-install --sync --update --yes && \
    xbps-install --yes git && \
    (cd /tmp; git clone https://github.com/void-linux/void-packages.git)

RUN xbps-install --yes gcc make bison flex gettext texinfo xz patch automake libtool pkg-config

RUN xbps-alternatives --set bash && \
    rm -rv /var/cache/xbps/* && \
    mv /etc/profile.d/colorls.sh /etc/profile.d/colorls.sh-disabled && \
    # remove non-docker content
    rm -rv /boot /home /media /mnt /opt \
    # try to refrain from /run
        /run

CMD [ "/bin/bash",  "-l" ]
