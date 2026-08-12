#include <bits/stdc++.h>
using namespace std;

struct Node{
    int value;
    Node* adressOfNextNode;
};

void insertAtLast(Node* &head, int value){
    if(head == nullptr){
        head = new Node;
        head->value = value;
        head->adressOfNextNode = nullptr;
    }
    else {
        Node* current = head;

        //traversing the node util the last node
        while(current->adressOfNextNode != nullptr){
            current = current->adressOfNextNode;
        }

        //creating a new node
        Node* temp = new Node;
        temp->value = value;
        temp->adressOfNextNode = nullptr;

        //connecting new node with last node
        current->adressOfNextNode = temp;
    }
    return;
}

void printSLL(Node* node){
    while(node != nullptr){
        cout << node->value << "->";
        node = node->adressOfNextNode;
    }
}

void removeNode(Node* &node, int index){
    if(node == nullptr) return;
    if(index == 0){
        Node* previousHead = node;
        node = node->adressOfNextNode;
        delete previousHead;
    }
    else{
        int currIndex = 0;
        Node* currNode = node;
        while(currIndex < index - 1 && currNode != nullptr){
            currNode = currNode->adressOfNextNode;
            currIndex++;
        }
        
        Node* targetNode = currNode->adressOfNextNode;
        if(targetNode == nullptr) return;
        if(targetNode->adressOfNextNode == nullptr){
            currNode->adressOfNextNode = nullptr;
            delete targetNode;
        }else {
            currNode->adressOfNextNode = targetNode->adressOfNextNode;
            delete targetNode;
        }
    }
}

void updateNode(Node* &node, int index, int value){
    if(node == nullptr) return;
    else{
        int currIndex =  0;
        Node* currentNode = node;
        while(currIndex < index && currentNode != nullptr){
            currentNode = currentNode->adressOfNextNode;
            currIndex++;
        }
        if(currIndex == index && currentNode != nullptr){
            currentNode->value = value;
        }
    }
}

int main(){
    ios::sync_with_stdio(0);
    cin.tie(0);

    Node* SLL = nullptr;
    insertAtLast(SLL, 10);
    insertAtLast(SLL, 20);
    insertAtLast(SLL, 30);
    insertAtLast(SLL, 40);
    printSLL(SLL);
    cout << "\n";

    //removeNode(SLL, 4);
    updateNode(SLL, 0, 100);
    
    printSLL(SLL);

    return 0;
}