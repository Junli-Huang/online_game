@echo off

.\protoc --csharp_out=. *.proto
.\protoc --descriptor_set_out descriptor_set *.proto

echo on
pause