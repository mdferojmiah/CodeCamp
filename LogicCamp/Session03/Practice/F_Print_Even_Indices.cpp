#include <bits/stdc++.h>
using namespace std;

void PrintEvenIndexReverseOrder(int arr[], int n){
    if(n < 0) return;
    if(n % 2 == 0) cout << arr[n] << " ";
    PrintEvenIndexReverseOrder(arr, n - 1);
}

int main(){
    ios::sync_with_stdio(0);
    cin.tie(0);

    int n; cin >> n;
    int arr[n];

    for(int i = 0; i < n; i++) cin >> arr[i];
    PrintEvenIndexReverseOrder(arr, n - 1);

    return 0;
}\