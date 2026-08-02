#include <bits/stdc++.h>
using namespace std;


void PrintDigits(int n){
    if(n < 10){
        cout << n;
        return;
    }
    PrintDigits(n / 10);
    cout << " " << n % 10;
}

int main(){
    ios::sync_with_stdio(0);
    cin.tie(0);

    int t; cin >> t;
    while(t--){
        int n; cin >> n;
        PrintDigits(n);
        cout << "\n";
    }

    return 0;
}