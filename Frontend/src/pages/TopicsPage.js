import React, { useEffect, useState } from 'react';
import { getAllBlocks, createBlock, deleteBlock, updateBlock } from '../api/blockService';
import { Link } from 'react-router-dom';

const BlockPage = () => {
  const [blocks, setBlocks] = useState([]);
  const [newBlockName, setNewBlockName] = useState('');
  const [editingBlockId, setEditingBlockId] = useState(null);
  const [editingBlockName, setEditingBlockName] = useState('');

  const fetchBlocks = async () => {
    try {
      const data = await getAllBlocks();
      console.log('Fetched blocks:', data); // Логирование полученных данных
      if (Array.isArray(data)) {
        setBlocks(data); // Если это массив, устанавливаем его в state
      } else {
        console.error("Unexpected data format:", data);
      }
    } catch (error) {
      console.error('Failed to fetch blocks', error);
    }
  };

  useEffect(() => {
    fetchBlocks();
  }, []);

  const handleCreateBlock = async () => {
    try {
      await createBlock({ title: newBlockName });
      setNewBlockName('');
      fetchBlocks();
    } catch (error) {
      console.error('Failed to create block', error);
    }
  };

  const handleDeleteBlock = async (id) => {
    try {
      await deleteBlock(id);
      fetchBlocks();
    } catch (error) {
      console.error('Failed to delete block', error);
    }
  };

  const handleEditBlock = (id, name) => {
    setEditingBlockId(id);
    setEditingBlockName(name);
  };

  const handleUpdateBlock = async () => {
    try {
      await updateBlock(editingBlockId, { title: editingBlockName });
      setEditingBlockId(null);
      setEditingBlockName('');
      fetchBlocks();
    } catch (error) {
      console.error('Failed to update block', error);
    }
  };

  return (
    <div className="p-4">
      <h1 className="text-2xl mb-4">Blocks</h1>

      {/* Create Block */}
      <div className="mb-6">
        <input
          type="text"
          value={newBlockName}
          onChange={(e) => setNewBlockName(e.target.value)}
          placeholder="New Block Name"
          className="border p-2 mr-2"
        />
        <button onClick={handleCreateBlock} className="bg-green-500 text-white p-2 rounded">
          Add Block
        </button>
      </div>

      {/* List Blocks */}
      <div className="space-y-4">
        {blocks.length > 0 ? (
          blocks.map((block) => (
            <div key={block.id} className="border p-4 rounded flex justify-between items-center">
              {editingBlockId === block.id ? (
                <div className="flex items-center gap-2">
                  <input
                    type="text"
                    value={editingBlockName}
                    onChange={(e) => setEditingBlockName(e.target.value)}
                    className="border p-1"
                  />
                  <button onClick={handleUpdateBlock} className="bg-blue-500 text-white p-1 rounded">
                    Save
                  </button>
                  <button onClick={() => setEditingBlockId(null)} className="bg-gray-300 p-1 rounded">
                    Cancel
                  </button>
                </div>
              ) : (
                <div className="flex items-center gap-2">
                  {/* Link to topics page */}
                  <Link to={`/materials/block/${block.id}`} className="text-blue-500 hover:underline">
                    Block {block.title}
                  </Link>
                  <button onClick={() => handleEditBlock(block.id, block.title)} className="bg-yellow-400 p-1 rounded">
                    Edit
                  </button>
                  <button onClick={() => handleDeleteBlock(block.id)} className="bg-red-500 text-white p-1 rounded">
                    Delete
                  </button>
                </div>
              )}
            </div>
          ))
        ) : (
          <p>No blocks available</p>
        )}
      </div>
    </div>
  );
};

export default BlockPage;