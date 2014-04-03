//#define ASTARDEBUG
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

namespace Pathfinding {
//Binary Heap
	
	
	/** Binary heap implementation. Binary heaps are really fast for ordering nodes in a way that makes it possible to get the node with the lowest F score. Also known as a priority queue.
	 * \see http://en.wikipedia.org/wiki/Binary_heap
	 */
	public class BinaryHeapM { 
		private PathNode[] binaryHeap; 
		public int numberOfItems; 
		
		public float growthFactor = 2;
		
		public BinaryHeapM ( int numberOfElements ) { 
			binaryHeap = new PathNode[numberOfElements]; 
			numberOfItems = 2;
		} 
		
		public void Clear () {
			numberOfItems = 1;
		}
		
		public PathNode GetNode (int i) {
			return binaryHeap[i];
		}
		
		/** Adds a node to the heap */
		public void Add(PathNode node) {
			
			if (node == null) throw new System.ArgumentNullException ("Sending null node to BinaryHeap");
			
			if (numberOfItems == binaryHeap.Length) {
				int newSize = System.Math.Max(binaryHeap.Length+4,(int)System.Math.Round(binaryHeap.Length*growthFactor));
				if (newSize > 1<<18) {
					throw new System.Exception ("Binary Heap Size really large (2^18). A heap size this large is probably the cause of pathfinding running in an infinite loop. " +
						"\nRemove this check (in BinaryHeap.cs) if you are sure that it is not caused by a bug");
				}
				
				PathNode[] tmp = new PathNode[newSize];
				for (int i=0;i<binaryHeap.Length;i++) {
					tmp[i] = binaryHeap[i];
				}
				binaryHeap = tmp;
				
				//Debug.Log ("Forced to discard nodes because of binary heap size limit, please consider increasing the size ("+numberOfItems +" "+binaryHeap.Length+")");
				//numberOfItems--;
			}
			
			binaryHeap[numberOfItems] = node;
			//node.heapIndex = numberOfItems;//Heap index
			
			int bubbleIndex = numberOfItems;
			uint nodeF = node.F;
			
			while (bubbleIndex != 1) {
				int parentIndex = bubbleIndex / 2;
				
				if (nodeF < binaryHeap[parentIndex].F) {
				   	
					//binaryHeap[bubbleIndex].f <= binaryHeap[parentIndex].f) { /* \todo Wouldn't it be more efficient with '<' instead of '<=' ? * /
					//Node tmpValue = binaryHeap[parentIndex];
					
					//tmpValue.heapIndex = bubbleIndex;//HeapIndex
					
					binaryHeap[bubbleIndex] = binaryHeap[parentIndex];
					binaryHeap[parentIndex] = node;//binaryHeap[bubbleIndex];
					
					//binaryHeap[bubbleIndex].heapIndex = bubbleIndex; //Heap index
					//binaryHeap[parentIndex].heapIndex = parentIndex; //Heap index
					
					bubbleIndex = parentIndex;
				} else {
				/*if (binaryHeap[bubbleIndex].f <= binaryHeap[parentIndex].f) { /* \todo Wouldn't it be more efficient with '<' instead of '<=' ? *
					Node tmpValue = binaryHeap[parentIndex];
					
					//tmpValue.heapIndex = bubbleIndex;//HeapIndex
					
					
					binaryHeap[parentIndex] = binaryHeap[bubbleIndex];
					binaryHeap[bubbleIndex] = tmpValue;
					
					bubbleIndex = parentIndex;
				} else {*/
					break;
				}
			}
								 
			numberOfItems++;
		}
		
		/** Returns the node with the lowest F score from the heap */
		public PathNode Remove() {
			numberOfItems--;
			PathNode returnItem = binaryHeap[1];
			
		 	//returnItem.heapIndex = 0;//Heap index
			
			binaryHeap[1] = binaryHeap[numberOfItems];
			//binaryHeap[1].heapIndex = 1;//Heap index
			
			int swapItem = 1, parent = 1;
			
			do {
				parent = swapItem;
				int p2 = parent * 2;
				if (p2 + 1 <= numberOfItems) {
					// Both children exist
					if (binaryHeap[parent].F >= binaryHeap[p2].F) {
						swapItem = p2;//2 * parent;
					}
					if (binaryHeap[swapItem].F >= binaryHeap[p2 + 1].F) {
						swapItem = p2 + 1;
					}
				} else if ((p2) <= numberOfItems) {
					// Only one child exists
					if (binaryHeap[parent].F >= binaryHeap[p2].F) {
						swapItem = p2;
					}
				}
				
				// One if the parent's children are smaller or equal, swap them
				if (parent != swapItem) {
					PathNode tmpIndex = binaryHeap[parent];
					//tmpIndex.heapIndex = swapItem;//Heap index
					
					binaryHeap[parent] = binaryHeap[swapItem];
					binaryHeap[swapItem] = tmpIndex;
					
					//binaryHeap[parent].heapIndex = parent;//Heap index
				}
			} while (parent != swapItem);
			
			return returnItem;
		}
		
		/** Rebuilds the heap by trickeling down all items.
		 * Usually called after the hTarget on a path has been changed */
		public void Rebuild () {
			
			for (int i=2;i<numberOfItems;i++) {
				int bubbleIndex = i;
				PathNode node = binaryHeap[i];
				uint nodeF = node.F;
				while (bubbleIndex != 1) {
					int parentIndex = bubbleIndex / 2;
					
					if (nodeF < binaryHeap[parentIndex].F) {
						//Node tmpValue = binaryHeap[parentIndex];
						binaryHeap[bubbleIndex] = binaryHeap[parentIndex];
						binaryHeap[parentIndex] = node;
						bubbleIndex = parentIndex;
					} else {
						break;
					}
				}
				
			}
			
			
		}
	}
}