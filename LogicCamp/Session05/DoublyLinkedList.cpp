#include <bits/stdc++.h>
using namespace std;


struct Node{
    int value;
    Node* next;
    Node* prev;

    Node(int value){
        this->value = value;
        this->next = nullptr;
        this->prev = nullptr;
    }
};

void insertAtTail(Node* &head, Node* &tail, int value){
    Node *newNode = new Node(value);

    if(head == nullptr){
        head = newNode;
        tail = newNode;
        return;
    }else{
        tail->next = newNode;
        newNode->prev = tail;
        tail = newNode;
    }
}

void insertAtIndex(Node* &head, Node* &tail, int index, int value){
    Node *newNode = new Node(value);
    
    //case 1: insert in the head
    if(index == 0){
        newNode->next =  head;
        head->prev = newNode;
        head = newNode;
        if(tail == nullptr){
            tail = newNode;
        }
        return;
    }

    Node *currNode = head;

    for(int i = 0; i < index - 1 && currNode != nullptr; i++){
        currNode = currNode->next;
    }

    if(currNode == nullptr){
        cout << "Index is out of the length!\n";
        delete newNode;
        return;
    }
    
    //case 2: insert in tail
    if(currNode == tail){
        insertAtTail(head, tail, value);
        delete newNode;
        return;
    }

    //case 3: insert in middle
    Node* nextNode = currNode->next;
    currNode->next = newNode;
    newNode->next = nextNode;
    nextNode->prev = newNode;
    newNode->prev = currNode;
}

void removeAtIndex(Node* &head, Node* &tail, int index){
    if(head == nullptr){
        cout << "Linked List already empty!\n";
        return;
    }
    if(index == 0){
        Node* targetNode = head;
        head = head->next;
        if(head == nullptr){
            tail = nullptr;
        }else{
            head->prev = nullptr;
        }
        
        delete targetNode;
        return;
    }

    Node* currNode = head;
    
    for(int i = 0; i < index - 1 && currNode != nullptr; i++){
        currNode = currNode->next;
    }

    if(currNode == nullptr || currNode->next == nullptr){
        cout << "Index is out of linked list!\n";
        return;
    }

    Node* targetNode = currNode->next;
    currNode->next =  targetNode->next;
    if(targetNode->next == nullptr){
        tail = currNode;
    } else{
        targetNode->next->prev = currNode;
    }
    delete targetNode;
}

void printDLL(Node *head){
    Node* currNode = head;
    while(currNode != nullptr){
        cout<< currNode->value;
        if(currNode->next != nullptr) cout << "->";
        currNode = currNode->next;
    }
    cout << "\n";
}

void printReverse(Node *tail){
    Node* currNode = tail;
    while(currNode != nullptr){
        cout << currNode->value;
        if(currNode->prev != nullptr) cout << "<-";
        currNode = currNode->prev;
    }
    cout << '\n';
}

int main(){
    ios::sync_with_stdio(0);
    cin.tie(0);

    int n;
    Node *head = nullptr;
    Node *tail = nullptr;

    while(cin >> n){
        insertAtTail(head, tail, n);
    }

    // insertAtTail(head, tail, 10);
    // insertAtTail(head, tail, 20);
    // insertAtTail(head, tail, 30);
    // insertAtTail(head, tail, 40);

    removeAtIndex(head, tail, 3);
    
    printDLL(head);
    
    // insertAtIndex(head, tail, 0, 100);

    // printDLL(head);
    printReverse(tail);

    return 0;
}
