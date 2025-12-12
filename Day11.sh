declare -A graph
while IFS=":" read n c; do
    graph[$n]=$c;
done
declare -A cache
linenum=0
dfs() {
    if [[ $1 == "out" ]]; then
        out[$4]=$(($2&&$3))
        # out[$4]=1
        return
    fi
    local hash=$1,$2,$3
    if [[ -z ${cache[$hash]} ]] ; then
        local fft=$2
        local dac=$3
        local next
        local d=$4 
        ((d++))
        [[ $1 == "fft" ]] && ((fft++))
        [[ $1 == "dac" ]] && ((dac++))
        # echo "> $hash"
        cache[$hash]=0
        for next in ${graph[$1]}; do
            dfs $next $fft $dac $d
            # [[ out[d] > 0 ]] && echo dfs $next $fft $dac $d ${out[$d]}
            ((cache[$hash]+=out[$d]))
        done
    fi
    # echo $4: $1 $2 $3 ${cache[$hash]}
    out[$4]=${cache[$hash]}
}

dfs svr 0 0 0
echo ${out[0]}
