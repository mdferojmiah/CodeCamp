#include <bits/stdc++.h>
using namespace std;

class Node{
    public:
        int val;
        int count;
        Node* left;
        Node* right;


        Node(int val){
            this->val = val;
            this->count = 1;
            this->left = nullptr;
            this->right = nullptr;
        }
};

Node* insert(Node* &node, int val){
    if(!node){
        Node* newNode = new Node(val);
        return newNode;
    }

    if(val > node->val){
        node->right = insert(node->right, val);
    }
    else if(val < node->val){
        node->left = insert(node->left, val);
    }
    else{
        node->count++;
    }
    return node;
}

void printBST(Node* node){
    if(!node) return;
    printBST(node->left);
    cout << node->val << " ";
    printBST(node->right);
}

bool search(Node* node, int val){
    if(!node) return false;

    if(node->val == val)
        return true;

    if(val > node->val){
        search(node->right, val);
    }else{
        search(node->left, val);
    }
}

Node* GetRightAnchester(Node* node){
    while(node->right){
        node = node->right;
    }
    return node;
}

Node* remove(Node* node, int target){
    if(!node) return node;

    if(target == node->val){
        // if count is more then 1, then need to decrease the count
        if(node->count > 1){
            node->count--;
            return node;
        }

        //case 1: leaf node
        if(!node->left && !node->right){
            delete node;
            return nullptr;
        }

        //case 2: has only one child
        if(!node->left){
            Node* rightChild = node->right;
            delete node;
            return rightChild;
        }else if(!node->right){
            Node* leftChild = node->left;
            delete node;
            return leftChild;
        }

        //case 3: has both child
        Node* anchester = GetRightAnchester(node->left);
        node->val = anchester->val;
        remove(node->left, anchester->val);
    }


    if(target > node->val){
        node->right = remove(node->right, target);
    }else{
        node->left = remove(node->left, target);
    }

    return node;
}

int main(){
    ios::sync_with_stdio(0);
    cin.tie(0);

    Node* node = nullptr;
    node = insert(node, 8);
    node = insert(node, 4);
    node = insert(node, 12);
    node = insert(node, 2);
    node = insert(node, 6);
    node = insert(node, 10);
    node = insert(node, 14);
    node = insert(node, 1);
    node = insert(node, 3);
    node = insert(node, 5);
    //node = insert(node, 7);
    node = insert(node, 9);
    node = insert(node, 11);
    node = insert(node, 13);
    node = insert(node, 15);

    printBST(node);

    if(search(node, 16)){
        cout << "\nfound\n";
    }else{
        cout << "\nnot found\n";
    }

    remove(node, 8);

    printBST(node);
    
    return 0;
}