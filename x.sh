#! /usr/bin/env bash
set -uvx
set -e
cwd=`pwd`
cd $cwd/EasyMediaPlayer
rm -rf obj bin
msbuild.exe -restore -verbosity:quiet
msbuild.exe -t:Pack -p:Configuration=Release -verbosity:quiet
