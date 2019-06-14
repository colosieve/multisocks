all: csharp cpp

clean:
	echo cleaned

cpp:
	-mkdir bin
	clang++ -o bin/multisocks_cpp src/cpp/main.cpp

csharp:
	cd src/csharp; dotnet build

.PHONY: all clean cpp csharp
