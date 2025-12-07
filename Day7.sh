mapfile G;W=${#G};H=${#G[@]}
for((Q=0;Q<H*W;Q++,x=Q%W));do ((o=x?o:0));g=${G[Q/W]:x:1}
[ $g = S ]&& A[$x]=1;[ $g = ^ ]&&((A[x-1]+=A[x],A[x+1]+=A[x],A[x]=0))||((o+=A[x]))
done;echo $o