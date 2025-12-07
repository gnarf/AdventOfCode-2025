#!/bin/bash

mapfile grid

W=${#grid}
H=${#grid[@]}
declare -A beams

for((y=0;y<H;y++)); do
    for((x=0;x<W;x++)); do
        g=${grid[$y]:$x:1}
        ((X=$x+$y*$W))
        [[ $g == 'S' ]] && ((beams[$X]=1))
        b=${beams[$X]}
        if [[ $b > 0 ]]; then
            if [[ $g == '^' ]]; then
                ((i=X-1+W))
                ((beams[$i]+=b))
                ((i=X+1+W))
                ((beams[$i]+=b))
            else
                ((i=X+W))
                ((beams[$i]+=b))
            fi
        fi
    done
done
total=0
for((x=(H-1)*W;x<W*H;x++)); do
    (( total+=beams[$x] ))
done
echo $total
