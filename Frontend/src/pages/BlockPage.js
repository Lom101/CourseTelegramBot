import React, { useEffect, useState } from 'react';
import { getAllBlocks, createBlock, deleteBlock, updateBlock } from '../api/blockService';
import { getAllTests } from '../api/testService'; // добавили импорт для тестов
import { Link } from 'react-router-dom';
import toast from 'react-hot-toast';

const BlockPage = () => {
  const [blocks, setBlocks] = useState([]);
  const [tests, setTests] = useState([]);
  const [newBlockName, setNewBlockName] = useState('');
  const [selectedTestId, setSelectedTestId] = useState('');
  const [editingBlockId, setEditingBlockId] = useState(null);
  const [editingBlockName, setEditingBlockName] = useState('');

  const fetchBlocks = async () => {
    try {
      const data = await getAllBlocks();
      if (Array.isArray(data)) {
        setBlocks(data);
      } else {
        console.error("Unexpected data format:", data);
      }
    } catch (error) {
      console.error('Failed to fetch blocks', error);
    }
  };

  const fetchTests = async () => {
    try {
      const data = await getAllTests();
      if (Array.isArray(data)) {
        setTests(data);
      } else {
        console.error("Unexpected data format for tests:", data);
      }
    } catch (error) {
      console.error('Failed to fetch tests', error);
    }
  };

  useEffect(() => {
    fetchBlocks();
    fetchTests();
  }, []);

  const handleCreateBlock = async () => {
    if (!selectedTestId) {
      toast.error('Пожалуйста, выберите финальный тест!');
      return;
    }

    try {
      await createBlock({
        title: newBlockName,
        finalTestId: selectedTestId ? Number(selectedTestId) : null,
      });
      setNewBlockName('');
      setSelectedTestId('');
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
    <div className="m-10">
      <h1 className="text-2xl mb-4">Главы</h1>

      {/* Create Block */}
      <div className="mb-6 flex items-center gap-4">
        <input
          type="text"
          value={newBlockName}
          onChange={(e) => setNewBlockName(e.target.value)}
          placeholder="Введите имя нового блока"
          className="border p-2 rounded w-auto"
        />

        <select
          value={selectedTestId}
          onChange={(e) => setSelectedTestId(e.target.value)}
          className="border p-2 rounded"
        >
          <option value="">Выберите финальный тест</option>
          {tests.map(test => (
            <option key={test.id} value={test.id}>
              {test.title}
            </option>
          ))}
        </select>

        <button onClick={handleCreateBlock} className="bg-green-500 text-white p-2 rounded">
          Добавить
        </button>
      </div>

      {/* List Blocks */}
      <div className="space-y-4">
        {blocks.length > 0 ? (
          blocks.map((block) => (
            <div key={block.id} className="border p-4 rounded flex justify-between items-center bg-white">
              {editingBlockId === block.id ? (
                <div className="flex items-center justify-between gap-4 w-full">
                  <input
                    type="text"
                    value={editingBlockName}
                    onChange={(e) => setEditingBlockName(e.target.value)}
                    className="border p-2 flex-1 rounded"
                  />
                  <div className="flex items-center gap-2">
                    <button onClick={handleUpdateBlock} className="bg-blue-500 text-white px-3 py-1 rounded">
                      Сохранить
                    </button>
                    <button onClick={() => setEditingBlockId(null)} className="bg-gray-300 px-3 py-1 rounded">
                      Отмена
                    </button>
                  </div>
                </div>
              ) : (
                <>
                  <Link to={`/blocks/${block.id}`} className="text-blue-500 hover:underline">
                    {block.title}
                  </Link>
                  <div className="flex items-center gap-2">
                    <button onClick={() => handleEditBlock(block.id, block.title)} className="bg-yellow-400 px-3 py-1 rounded">
                      Редактировать
                    </button>
                    <button onClick={() => handleDeleteBlock(block.id)} className="bg-red-500 text-white px-3 py-1 rounded">
                      Удалить
                    </button>
                  </div>
                </>
              )}
            </div>
          ))
        ) : (
          <p>Еще не добавлено ни одного блока(главы)</p>
        )}
      </div>
    </div>
  );
};

export default BlockPage;
