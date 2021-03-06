#!/bin/bash

shopt -s checkwinsize
umask 022
export HISTCONTROL=ignorespace:ignoredups:erasedups

export POGO_ENV="pogo.void"
export PATH=/sbin:/bin:/usr/sbin:/usr/bin:/root/McMorph/tools
export TERM=xterm
export LANG=C.UTF-8
export LANGUAGE=C.UTF-8
export LC_ALL=C.UTF-8
export FORCE_UNSAFE_CONFIGURE=1
export USER=root

export PAGER=less
export LESS=-XR

export PS1="($POGO_ENV) [\u]: \w\\$ "
export PROMPT_COMMAND='echo -ne "\033]0;($POGO_ENV) [${USER}]: ${PWD}\007"'

alias l='ls -lA'
alias ll='ls -lA'
alias lll='ls -lA | less'
alias cls='clear'
