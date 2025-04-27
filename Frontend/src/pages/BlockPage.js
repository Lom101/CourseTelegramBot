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
    <div className="min-h-screen bg-gray-50 px-10 py-8">
  <h1 className="text-3xl font-extrabold mb-6 text-gray-800 mt-12">Главы</h1>

  {/* Create Block */}
  <div className="mb-8 flex items-center gap-4">
    <input
      type="text"
      value={newBlockName}
      onChange={(e) => setNewBlockName(e.target.value)}
      placeholder="Введите имя нового блока"
      className="border border-gray-300 p-3 rounded-lg w-auto text-gray-800 placeholder-gray-500 focus:outline-none focus:ring-2 focus:ring-gray-500"
    />

    <select
      value={selectedTestId}
      onChange={(e) => setSelectedTestId(e.target.value)}
      className="border border-gray-300 p-3 rounded-lg text-gray-800 focus:outline-none focus:ring-2 focus:ring-gray-500"
    >
      <option value="">Выберите финальный тест</option>
      {tests.map(test => (
        <option key={test.id} value={test.id}>
          {test.title}
        </option>
      ))}
    </select>

    <button
      onClick={handleCreateBlock}
      className="bg-gray-600 text-white p-3 rounded-lg hover:bg-gray-700 transition"
    >
      Добавить
    </button>
  </div>

  {/* List Blocks */}
  <div className="space-y-6">
    {blocks.length > 0 ? (
      blocks.map((block) => (
        <div
          key={block.id}
          className="border border-gray-300 p-4 rounded-lg bg-white flex justify-between items-center shadow-sm hover:shadow-lg transition"
        >
          {editingBlockId === block.id ? (
            <div className="flex items-center justify-between gap-4 w-full">
              <input
                type="text"
                value={editingBlockName}
                onChange={(e) => setEditingBlockName(e.target.value)}
                className="border border-gray-300 p-3 flex-1 rounded-lg focus:outline-none focus:ring-2 focus:ring-gray-500"
              />
              <div className="flex items-center gap-2">
                <button
                  onClick={handleUpdateBlock}
                  className="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 transition"
                >
                  Сохранить
                </button>
                <button
                  onClick={() => setEditingBlockId(null)}
                  className="bg-gray-300 text-gray-800 px-4 py-2 rounded-lg hover:bg-gray-400 transition"
                >
                  Отмена
                </button>
              </div>
            </div>
          ) : (
            <>
              <Link
                to={`/blocks/${block.id}`}
                className="text-gray-700 hover:text-blue-500 font-semibold"
              >
                {block.title}
              </Link>
              <div className="flex items-center gap-3">
                <button
                  onClick={() => handleEditBlock(block.id, block.title)}
                  className="bg-gray-300 text-gray-800 px-4 py-2 rounded-lg hover:bg-gray-400 transition"
                >
                  Редактировать
                </button>
                <button
                  onClick={() => handleDeleteBlock(block.id)}
                  className="bg-red-400 text-white px-4 py-2 rounded-lg hover:bg-red-500 transition"
                >
                  Удалить
                </button>
              </div>
            </>
          )}
        </div>
      ))
    ) : (
      <p className="text-gray-600">Еще не добавлено ни одного блока (главы)</p>
    )}
  </div>
</div>

  );
};

export default BlockPage;
