for filename in test/integration/*.sh; do
    name=${filename##*/}
    actual="test/integration/actual/${name}.log"
    expected="test/integration/expected/${name}.log"
    echo "Execute: ${name}"
    cd bin/dotnet && \
    sh "../../${filename}" > "../../${actual}" && \
    cd ../..
    code=$?
    if [ $code -ne 0 ];
    then
        echo "Result : Runtime error $code"
        exit 1
    else
        diff -a "${actual}" "${expected}"
        code=$?
        if [ $code -ne 0 ];
        then
            echo "Result : Diff error $code"
            exit 1
        else
            echo "Result : OK" 
        fi
    fi
done
echo "Passed !"
