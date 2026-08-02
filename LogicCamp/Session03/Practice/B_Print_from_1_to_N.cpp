#include <bits/stdc++.h>
using namespace std;

void Print(int n){
    if(n == 0) return;
    Print(n - 1);
    cout << n << "\n";
}

int main(){
    ios::sync_with_stdio(0);
    cin.tie(0);

    int n; cin>> n;
    Print(n);

    return 0;
}