import { useState } from "react";
import axios from "axios";

const VideoUpload = ({ onUpload }) => {
  const [file, setFile] = useState(null);
  const [uploading, setUploading] = useState(false);

  const handleFileChange = (e) => setFile(e.target.files[0]);

  const uploadVideo = async () => {
    if (!file) return;
    setUploading(true);
    const formData = new FormData();
    formData.append("file", file);

    try {
      await axios.post("http://localhost:5000/upload", formData);
      onUpload();
    } catch (error) {
      console.error("Upload failed", error);
    } finally {
      setUploading(false);
    }
  };

  return (
    <div className="p-4 border rounded-lg shadow">
      <input type="file" accept="video/*" onChange={handleFileChange} className="mb-2" />
      <button onClick={uploadVideo} disabled={uploading} className="px-4 py-2 bg-blue-500 text-white rounded">
        {uploading ? "Uploading..." : "Upload Video"}
      </button>
    </div>
  );
};

export default VideoUpload;
