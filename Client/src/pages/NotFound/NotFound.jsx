import React from 'react';
import { FolderX } from 'lucide-react';

function NotFound() {
  return (
    <div className="min-h-screen bg-gray-900 flex items-center justify-center">
      <div className="w-3/4 bg-gray-800 p-12 rounded-2xl shadow-2xl backdrop-blur-sm border border-gray-700 text-center">
        <div className="flex justify-center mb-8">
          <div className="p-6 rounded-full bg-gray-700">
            <FolderX className="h-24 w-24 text-gray-400" />
          </div>
        </div>
        <h1 className="text-7xl font-bold text-gray-300 mb-4 tracking-tighter">404</h1>
        <h2 className="text-2xl text-gray-400 mb-6 font-light">Page Not Found</h2>
        <p className="text-gray-500 text-lg mb-10 max-w-lg mx-auto">
          The page you're looking for seems to have vanished into the digital void.
        </p>
        <a
          href="/login"
          className="inline-flex items-center px-8 py-3 rounded-xl bg-gray-600 text-white font-medium 
                   hover:bg-gray-500 transition-all duration-200 ease-in-out 
                   shadow-lg shadow-gray-600/30 hover:shadow-gray-500/40
                   hover:scale-105"
        >
          Return Home
        </a>
      </div>
    </div>
  );
}

export default NotFound;