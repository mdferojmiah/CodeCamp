#include <bits/stdc++.h>
using namespace std;

void Binary(int n){
    if(n < 1) return;
    Binary(n / 2);
    cout << n % 2;
}

int main(){
    ios::sync_with_stdio(0);
    cin.tie(0);

    int t; cin >> t;
    while(t--){
        int n; cin >> n;
        Binary(n);
        cout << "\n";
    }

    return 0;
}