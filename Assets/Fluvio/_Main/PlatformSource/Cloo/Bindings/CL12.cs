#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID
using System;

namespace Cloo.Bindings
{
    [System.Security.SuppressUnmanagedCodeSecurity]
    public class CL12 : CL11, ICL12
    {
        [Obsolete("Deprecated in OpenCL 1.2.")]
        CLMemoryHandle ICL12.CreateFromGLTexture2D(CLContextHandle context, ComputeMemoryFlags flags, int target, int miplevel,
                                                   int texture, out ComputeErrorCode errcode_ret)
        {
            return ((ICL10) this).CreateFromGLTexture2D(context, flags, target, miplevel, texture, out errcode_ret);
        }
        [Obsolete("Deprecated in OpenCL 1.2.")]
        CLMemoryHandle ICL12.CreateFromGLTexture3D(CLContextHandle context, ComputeMemoryFlags flags, int target, int miplevel,
                                                   int texture, out ComputeErrorCode errcode_ret)
        {
            return ((ICL10) this).CreateFromGLTexture3D(context, flags, target, miplevel, texture, out errcode_ret);
        }
        [Obsolete("Deprecated in OpenCL 1.2.")]
        CLMemoryHandle ICL12.CreateImage2D(CLContextHandle context, ComputeMemoryFlags flags, ref ComputeImageFormat image_format,
                                           IntPtr image_width, IntPtr image_height, IntPtr image_row_pitch, IntPtr host_ptr,
                                           out ComputeErrorCode errcode_ret)
        {
            return ((ICL10)this).CreateImage2D(context, flags, ref image_format, image_width, image_height, image_row_pitch, host_ptr, out errcode_ret);
        }
        [Obsolete("Deprecated in OpenCL 1.2.")]
        CLMemoryHandle ICL12.CreateImage3D(CLContextHandle context, ComputeMemoryFlags flags, ref ComputeImageFormat image_format,
                                           IntPtr image_width, IntPtr image_height, IntPtr image_depth, IntPtr image_row_pitch,
                                           IntPtr image_slice_pitch, IntPtr host_ptr, out ComputeErrorCode errcode_ret)
        {
            return ((ICL10)this).CreateImage3D(context, flags, ref image_format, image_width, image_height, image_depth, image_row_pitch, image_slice_pitch, host_ptr, out errcode_ret);
        }
        [Obsolete("Deprecated in OpenCL 1.2.")]
        ComputeErrorCode ICL12.EnqueueWaitForEvents(CLCommandQueueHandle command_queue, int num_events, CLEventHandle[] event_list)
        {
            return ((ICL10) this).EnqueueWaitForEvents(command_queue, num_events, event_list);
        }
        [Obsolete("Deprecated in OpenCL 1.2.")]
        IntPtr ICL12.GetExtensionFunctionAddress(string func_name)
        {
            return ((ICL10) this).GetExtensionFunctionAddress(func_name);
        }
        [Obsolete("Deprecated in OpenCL 1.2.")]
        ComputeErrorCode ICL12.UnloadCompiler()
        {
            return ((ICL10) this).UnloadCompiler();
        }
        [Obsolete("Deprecated in OpenCL 1.2.")]
        ComputeErrorCode ICL12.EnqueueBarrier(CLCommandQueueHandle command_queue)
        {
            return ((ICL10) this).EnqueueBarrier(command_queue);
        }
        [Obsolete("Deprecated in OpenCL 1.2.")]
        ComputeErrorCode ICL12.EnqueueMarker(CLCommandQueueHandle command_queue, out CLEventHandle new_event)
        {
            return ((ICL10) this).EnqueueMarker(command_queue, out new_event);
        }
    }
}
#endif
