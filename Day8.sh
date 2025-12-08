mapfile -t J;n=${#J[@]};IFS=', '
for((a=0;a<n;C[a]=a++));do for((b=a;++b<n;));do A=(${J[a]} ${J[b]})
echo $[i=A[0]-A[3],j=A[1]-A[4],k=A[2]-A[5],i*i+j*j+k*k] $a $b
done;done>_;while read d a b;do
for((v=C[a],w=C[b],x=l=0;x<n;C[x]==w?C[x]=v:0,l+=C[x++]==v));do :;done
[ $l = $n ]&&echo $[${J[a]/,*}*${J[b]/,*}]&&exit;done< <(sort -n _)