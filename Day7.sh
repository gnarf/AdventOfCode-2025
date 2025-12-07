mapfile G;W=${#G};H=${#G[@]}
for((Q=0;Q<H*W;Q++));do
((o=Q%W?o:0))
g=${G[Q/W]:Q%W:1}
[[ $g == 'S' ]]&& A[$Q]=1
[[ $g == '^' ]]&&((A[Q-1+W]+=A[Q],A[Q+1+W]+=A[Q]))||((A[Q+W]+=t,o+=A[Q]))
done
echo $o